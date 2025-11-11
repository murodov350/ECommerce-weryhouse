import { useEffect, useState } from 'react';
import { apiFetch } from '../auth.ts';

interface Role { id: string; name: string; }
interface Permission { id: string; module: string; action: string; description?: string; }

const baseModules = ['User','Role'];
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

  function refreshPermissions(){
    apiFetch('/api/permissions/catalog').then(async r=>{ if(r.ok) setPermissions(await r.json()); });
  }

  async function loadRolePerms(roleId:string){
    const r = await apiFetch('/api/permissions/role/'+roleId);
    if(r.ok){ const ids:string[] = await r.json(); setRolePerms(new Set(ids)); }
  }

  const toggle = (permId:string)=>{
    const next = new Set(rolePerms);
    if(next.has(permId)) next.delete(permId); else next.add(permId);
    setRolePerms(next);
  };

  async function generateBase(){
    // Ensure base CRUD permissions exist
    for(const m of baseModules){
      for(const a of crud){
        const exists = permissions.find(p=>p.module===m && p.action===a);
        if(!exists){
          await apiFetch('/api/permissions/catalog',{ method:'POST', body: JSON.stringify({ module:m, action:a }) });
        }
      }
    }
    refreshPermissions();
  }

  async function save(){
    if(!selectedRole) return;
    setSaving(true);
    const filtered = permissions.filter(p=> rolePerms.has(p.id));
    await apiFetch('/api/permissions/assign',{ method:'POST', body: JSON.stringify({ roleId: selectedRole, permissionIds: filtered.map(f=>f.id) }) });
    setSaving(false);
  }

  return (
    <div style={{marginTop:'2rem'}}>
      <h2>Permissions Manager</h2>
      <div style={{display:'flex', gap:'1rem', flexWrap:'wrap'}}>
        <div>
          <label>Role:</label><br/>
          <select value={selectedRole} onChange={e=>setSelectedRole(e.target.value)}>
            <option value="">-- select --</option>
            {roles.map(r=> <option key={r.id} value={r.id}>{r.name}</option>)}
          </select>
        </div>
        <button className="btn primary" onClick={generateBase}>Generate Base CRUD</button>
        <button className="btn primary" onClick={save} disabled={!selectedRole || saving}>{saving? 'Saving...':'Save'}</button>
      </div>

      <table className="table" style={{marginTop:'1rem'}}>
        <thead><tr><th>Module</th><th>Action</th><th>Assign</th></tr></thead>
        <tbody>
          {permissions.map(p=> (
            <tr key={p.id}>
              <td>{p.module}</td>
              <td>{p.action}</td>
              <td>
                {selectedRole && <input type="checkbox" checked={rolePerms.has(p.id)} onChange={()=>toggle(p.id)} />}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
