import { useCallback, useEffect, useState } from 'react';
import { CrudTable } from '../components/CrudTable';
import { getProducts, createProduct, updateProduct, deleteProduct, getCategories } from '../components/api';

export default function ProductsPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', sku:'', price:0, quantity:0, categoryId:'' });
  const [categories,setCategories]=useState<any[]>([]);

  useEffect(()=>{ getCategories().then((c:any)=> setCategories(c.items||c)); },[]);

  const fetchItems = useCallback(()=> getProducts(), []);

  const onEdit = (row:any)=>{ setEditing(row); setForm({ name: row.name, sku: row.sku, price: row.price, quantity: row.quantity, categoryId: row.categoryId }); };
  const onDelete = async (row:any)=>{ await deleteProduct(row.id); window.location.reload(); };

  const onSave = async ()=>{
    if(editing){ await updateProduct(editing.id, {...form, price:Number(form.price), quantity:Number(form.quantity)}); } else { await createProduct({...form, price:Number(form.price), quantity:Number(form.quantity)}); }
    window.location.reload();
  };

  return (
    <div>
      <CrudTable title="Products" fetchItems={fetchItems} onEdit={onEdit} onDelete={onDelete}
        columns={[{key:'name', header:'Name'},{key:'sku', header:'SKU'},{key:'price', header:'Price'},{key:'quantity', header:'Qty'},{key:'categoryName', header:'Category'}]}
        toolbar={<button className="btn primary" onClick={()=>{ setEditing(null); setForm({name:'',sku:'',price:0,quantity:0,categoryId:''}); }}>New</button>}
      />
      {(editing!==undefined) && (
        <div style={{marginTop:'1rem'}}>
          <h3>{editing? 'Edit':'Create'} Product</h3>
          <div style={{display:'flex', gap:'.5rem', flexWrap:'wrap'}}>
            <input placeholder="Name" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/>
            <input placeholder="SKU" value={form.sku} onChange={e=>setForm({...form, sku:e.target.value})}/>
            <input placeholder="Price" type="number" value={form.price} onChange={e=>setForm({...form, price:e.target.value as any})}/>
            <input placeholder="Quantity" type="number" value={form.quantity} onChange={e=>setForm({...form, quantity:e.target.value as any})}/>
            <select value={form.categoryId} onChange={e=>setForm({...form, categoryId:e.target.value})}>
              <option value="">-- Category --</option>
              {categories.map(c=> <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
            <button className="btn primary" onClick={onSave}>Save</button>
          </div>
        </div>
      )}
    </div>
  );
}
