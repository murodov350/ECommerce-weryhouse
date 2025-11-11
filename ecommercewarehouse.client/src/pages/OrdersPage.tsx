import { useCallback, useEffect, useState } from 'react';
import { CrudTable } from '../components/CrudTable';
import { createOrder, getOrders, getProducts, updateOrderStatus } from '../components/api';

export default function OrdersPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ customerName:'', items:[] as any[] });
  const [products,setProducts]=useState<any[]>([]);

  useEffect(()=>{ getProducts().then((p:any)=> setProducts(p.items||p)); },[]);

  const fetchItems = useCallback(()=> getOrders(), []);

  const onEdit = (row:any)=>{ setEditing(row); };

  const onSave = async ()=>{
    await createOrder(form); window.location.reload();
  };

  const setItem = (idx:number, patch:any)=>{
    const items = [...form.items]; items[idx] = { ...(items[idx]||{productId:'', quantity:1, unitPrice:0}), ...patch }; setForm({...form, items});
  };

  return (
    <div>
      <CrudTable title="Orders" fetchItems={fetchItems} onEdit={onEdit}
        columns={[{key:'orderNumber', header:'Order #'}, {key:'customerName', header:'Customer'}, {key:'totalAmount', header:'Total'}, {key:'status', header:'Status'}]}
        actions={(row:any)=> (
          <select defaultValue={row.status} onChange={async e=>{ await updateOrderStatus(row.id, e.target.value); window.location.reload(); }}>
            {['Pending','Paid','Shipped','Cancelled'].map(s=> <option key={s} value={s}>{s}</option>)}
          </select>
        )}
        toolbar={<button className="btn primary" onClick={()=>{ setEditing(null); setForm({customerName:'', items:[]}); }}>New</button>}
      />
      {(editing!==undefined) && (
        <div style={{marginTop:'1rem'}}>
          <h3>Create Order</h3>
          <div style={{display:'flex', gap:'.5rem', flexDirection:'column'}}>
            <input placeholder="Customer Name" value={form.customerName} onChange={e=>setForm({...form, customerName:e.target.value})}/>
            <div>
              <button className="btn" onClick={()=> setForm({...form, items:[...form.items, {productId:'', quantity:1, unitPrice:0}]})}>Add Item</button>
            </div>
            {form.items.map((it:any, idx:number)=>(
              <div key={idx} style={{display:'flex', gap:'.5rem', alignItems:'center'}}>
                <select value={it.productId} onChange={e=> setItem(idx, { productId:e.target.value })}>
                  <option value="">-- Product --</option>
                  {products.map(p=> <option key={p.id} value={p.id}>{p.name}</option>)}
                </select>
                <input placeholder="Qty" type="number" value={it.quantity} onChange={e=> setItem(idx, { quantity:Number(e.target.value) })} />
                <input placeholder="Unit Price" type="number" value={it.unitPrice} onChange={e=> setItem(idx, { unitPrice:Number(e.target.value) })} />
              </div>
            ))}
            <button className="btn primary" onClick={onSave}>Save Order</button>
          </div>
        </div>
      )}
    </div>
  );
}
