import { useCallback, useState, useEffect } from 'react';
import { CrudTable } from '../components/CrudTable';
import { createCategory, deleteCategory, getCategories, updateCategory } from '../components/api';

export default function CategoriesPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', description:'' });
  const [refreshKey,setRefreshKey]=useState(0);
  const [items,setItems]=useState<any[]>([]);

  const load = useCallback(async ()=>{
    const data = await getCategories();
    setItems(data.items || data); // handle paged vs list
  },[]);

  useEffect(()=>{ load(); },[refreshKey, load]);

  const onEdit = (row:any)=>{ setEditing(row); setForm({ name: row.name, description: row.description||''}); };
  const onDelete = async (row:any)=>{ await deleteCategory(row.id); setRefreshKey(k=>k+1); };

  const onSave = async ()=>{
    if(editing){ await updateCategory(editing.id, form); } else { await createCategory(form); }
    setEditing(null); setForm({name:'',description:''}); setRefreshKey(k=>k+1);
  };

  return (
    <div>
      <CrudTable title="Categories" fetchItems={async()=>items} onEdit={onEdit} onDelete={onDelete}
        columns={[{key:'name', header:'Name'}, {key:'description', header:'Description'}]}
        toolbar={<button className="btn primary" onClick={()=>{ setEditing(null); setForm({name:'',description:''}); }}>New</button>}
      />
      {editing!==undefined && (
        <div className="card-form">
          <h3>{editing? 'Edit':'Create'} Category</h3>
          <div className="form-grid">
            <input placeholder="Name" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/>
            <input placeholder="Description" value={form.description} onChange={e=>setForm({...form, description:e.target.value})}/>
            <button className="btn primary" onClick={onSave}>Save</button>
            {editing && <button className="btn" onClick={()=>{ setEditing(null); setForm({name:'',description:''}); }}>Cancel</button>}
          </div>
        </div>
      )}
    </div>
  );
}
