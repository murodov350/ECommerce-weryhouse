import { useCallback, useState, useEffect } from 'react';
import { CrudTable } from '../components/CrudTable';
import { getSuppliers, createSupplier, updateSupplier, deleteSupplier } from '../components/api';

export default function SuppliersPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', phone:'', email:'' });
  const [refreshKey,setRefreshKey]=useState(0);
  const [items,setItems]=useState<any[]>([]);

  const load = useCallback(async ()=>{
    const data = await getSuppliers();
    setItems(data.items || data);
  },[]);

  useEffect(()=>{ load(); },[refreshKey, load]);

  const onEdit = (row:any)=>{ setEditing(row); setForm({ name: row.name, phone: row.phone||'', email: row.email||''}); };
  const onDelete = async (row:any)=>{ await deleteSupplier(row.id); setRefreshKey(k=>k+1); };

  const onSave = async ()=>{
    if(editing){ await updateSupplier(editing.id, form); } else { await createSupplier(form); }
    setEditing(null); setForm({name:'',phone:'',email:''}); setRefreshKey(k=>k+1);
  };

  return (
    <div>
      <CrudTable title="Suppliers" fetchItems={async()=>items} onEdit={onEdit} onDelete={onDelete}
        columns={[{key:'name', header:'Name'},{key:'phone', header:'Phone'},{key:'email', header:'Email'}]}
        toolbar={<button className="btn primary" onClick={()=>{ setEditing(null); setForm({name:'',phone:'',email:''}); }}>New</button>}
      />
      {editing!==undefined && (
        <div className="card-form">
          <h3>{editing? 'Edit':'Create'} Supplier</h3>
          <div className="form-grid">
            <input placeholder="Name" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/>
            <input placeholder="Phone" value={form.phone} onChange={e=>setForm({...form, phone:e.target.value})}/>
            <input placeholder="Email" value={form.email} onChange={e=>setForm({...form, email:e.target.value})}/>
            <button className="btn primary" onClick={onSave}>Save</button>
            {editing && <button className="btn" onClick={()=>{ setEditing(null); setForm({name:'',phone:'',email:''}); }}>Cancel</button>}
          </div>
        </div>
      )}
    </div>
  );
}
