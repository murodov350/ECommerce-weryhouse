import { useCallback, useEffect, useState } from 'react';
import { CrudTable } from '../components/CrudTable';
import { getProducts, createProduct, updateProduct, deleteProduct, getCategories } from '../components/api';

export default function ProductsPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', sku:'', price:0, quantity:0, categoryId:'' });
  const [categories,setCategories]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [refreshKey,setRefreshKey]=useState(0);

  useEffect(()=>{ getCategories().then((c:any)=> setCategories(c.items||c)); },[]);

  const load = useCallback(async ()=>{
    const data = await getProducts();
    setItems(data.items || data);
  },[]);

  useEffect(()=>{ load(); },[refreshKey, load]);

  const onEdit = (row:any)=>{ setEditing(row); setForm({ name: row.name, sku: row.sku, price: row.price, quantity: row.quantity, categoryId: row.categoryId }); };
  const onDelete = async (row:any)=>{ await deleteProduct(row.id); setRefreshKey(k=>k+1); };

  const onSave = async ()=>{
    const payload = { ...form, price:Number(form.price), quantity:Number(form.quantity) };
    if(editing){ await updateProduct(editing.id, payload); } else { await createProduct(payload); }
    setEditing(null); setForm({name:'',sku:'',price:0,quantity:0,categoryId:''}); setRefreshKey(k=>k+1);
  };

  return (
    <div>
      <CrudTable title="Products" fetchItems={async()=>items} onEdit={onEdit} onDelete={onDelete}
        columns={[{key:'name', header:'Name'},{key:'sku', header:'SKU'},{key:'price', header:'Price'},{key:'quantity', header:'Qty'},{key:'categoryName', header:'Category'}]}
        toolbar={<button className="btn primary" onClick={()=>{ setEditing(null); setForm({name:'',sku:'',price:0,quantity:0,categoryId:''}); }}>New</button>}
      />
      {editing!==undefined && (
        <div className="card-form">
          <h3>{editing? 'Edit':'Create'} Product</h3>
          <div className="form-grid">
            <input placeholder="Name" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/>
            <input placeholder="SKU" value={form.sku} onChange={e=>setForm({...form, sku:e.target.value})}/>
            <input placeholder="Price" type="number" value={form.price} onChange={e=>setForm({...form, price:e.target.value as any})}/>
            <input placeholder="Quantity" type="number" value={form.quantity} onChange={e=>setForm({...form, quantity:e.target.value as any})}/>
            <select value={form.categoryId} onChange={e=>setForm({...form, categoryId:e.target.value})}>
              <option value="">-- Category --</option>
              {categories.map(c=> <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
            <button className="btn primary" onClick={onSave}>Save</button>
            {editing && <button className="btn" onClick={()=>{ setEditing(null); setForm({name:'',sku:'',price:0,quantity:0,categoryId:''}); }}>Cancel</button>}
          </div>
        </div>
      )}
    </div>
  );
}
