import { useEffect, useMemo, useState } from 'react';
import { apiFetch } from '../auth.ts';

declare global { interface Window { toastr:any; bootstrap:any } }

interface Role { id: string; name: string; }
interface Permission { id: string; module: string; action: string; description?: string; }

const crud = ['View','Create','Update','Delete'];

export default function PermissionsManager(){
  const [roles,setRoles]=useState<Role[]>([]);
  const [permissions,setPermissions]=useState<Permission[]>([]);
  const [selectedRole,setSelectedRole]=useState<string>('');
  const [rolePerms,setRolePerms]=useState<Set<string>>(new Set());
  const [saving,setSaving]=useState(false);

  useEffect(()=>{
    apiFetch('/api/roles').then(async r=>{ if(r.ok) setRoles(await r.json()); });
    refreshPermissions();
  },[]);

  useEffect(()=>{ if(selectedRole) loadRolePerms(selectedRole); },[selectedRole]);

  function refreshPermissions(){ apiFetch('/api/permissions/catalog').then(async r=>{ if(r.ok) setPermissions(await r.json()); }); }
  async function loadRolePerms(roleId:string){ const r=await apiFetch('/api/permissions/role/'+roleId); if(r.ok){ const ids:string[]=await r.json(); setRolePerms(new Set(ids)); } }

  const modules = useMemo(()=> Array.from(new Set(permissions.map(p=>p.module))).sort(), [permissions]);
  const mapBy = (m:string,a:string)=> permissions.find(p=> p.module===m && p.action===a);

  const toggle = (permId:string|undefined)=>{
    if(!permId) return; const next = new Set(rolePerms); if(next.has(permId)) next.delete(permId); else next.add(permId); setRolePerms(next);
    window.toastr?.success('Updated selection');
  };

  async function save(){ if(!selectedRole) return; setSaving(true); const filtered = permissions.filter(p=> rolePerms.has(p.id));
    const res = await apiFetch('/api/permissions/assign',{ method:'POST', body: JSON.stringify({ roleId: selectedRole, permissionIds: filtered.map(f=>f.id) }) });
    setSaving(false);
    if(res.ok) window.toastr?.success('Permissions saved'); else window.toastr?.error('Save failed');
  }

  return (
    <div className="container-fluid py-2">
      <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
        <h2 className="mb-0">Permissions</h2>
        <div className="d-flex gap-2">
          <select className="form-select" value={selectedRole} onChange={e=>setSelectedRole(e.target.value)}>
            <option value="">Select role</option>
            {roles.map(r=> <option key={r.id} value={r.id}>{r.name}</option>)}
          </select>
          <button className="btn btn-primary" disabled={!selectedRole || saving} onClick={save}>{saving? 'Saving...':'Save'}</button>
        </div>
      </div>

      <div className="table-responsive">
        <table className="table table-bordered align-middle">
          <thead className="table-light">
            <tr>
              <th style={{minWidth:180}}>Module</th>
              {crud.map(a=> <th key={a} className="text-center" style={{width:120}}>{a}</th>)}
            </tr>
          </thead>
          <tbody>
            {modules.map(m=> (
              <tr key={m}>
                <td className="fw-semibold">{m}</td>
                {crud.map(a=>{
                  const perm = mapBy(m,a);
                  const checked = perm? rolePerms.has(perm.id): false;
                  return (
                    <td key={a} className="text-center">
                      <input type="checkbox" className="form-check-input" disabled={!selectedRole || !perm} checked={checked} onChange={()=> toggle(perm?.id)} />
                    </td>
                  );
                })}
              </tr>
            ))}
            {modules.length===0 && (
              <tr><td colSpan={1+crud.length} className="text-center text-muted">No permissions</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
