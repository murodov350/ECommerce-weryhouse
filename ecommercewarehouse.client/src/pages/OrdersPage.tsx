import { useCallback, useEffect, useState } from 'react';
import { CrudTable } from '../components/CrudTable';
import { createOrder, getOrders, getProducts, updateOrderStatus } from '../components/api';

export default function OrdersPage(){
  const [form,setForm]=useState({ customerName:'', items:[] as any[] });
  const [products,setProducts]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [refreshKey,setRefreshKey]=useState(0);

  useEffect(()=>{ getProducts().then((p:any)=> setProducts(p.items||p)); },[]);

  const load = useCallback(async ()=>{ const data = await getOrders(); setItems(data.items||data); },[]);
  useEffect(()=>{ load(); },[refreshKey, load]);

  const onSave = async ()=>{ await createOrder(form); setForm({customerName:'', items:[]}); setRefreshKey(k=>k+1); };

  const setItem = (idx:number, patch:any)=>{
    const its = [...form.items]; its[idx] = { ...(its[idx]||{productId:'', quantity:1, unitPrice:0}), ...patch }; setForm({...form, items:its});
  };

  return (
    <div>
      <CrudTable title="Orders" fetchItems={async()=>items}
        columns={[{key:'orderNumber', header:'Order #'}, {key:'customerName', header:'Customer'}, {key:'totalAmount', header:'Total'}, {key:'status', header:'Status'}]}
        actions={(row:any)=> (
          <select defaultValue={row.status} onChange={async e=>{ await updateOrderStatus(row.id, e.target.value); setRefreshKey(k=>k+1); }}>
            {['Pending','Paid','Shipped','Cancelled'].map(s=> <option key={s} value={s}>{s}</option>)}
          </select>
        )}
        toolbar={<></>}
      />
      <div className="card-form">
        <h3>Create Order</h3>
        <div className="form-grid">
          <input placeholder="Customer Name" value={form.customerName} onChange={e=>setForm({...form, customerName:e.target.value})}/>
          <button className="btn" onClick={()=> setForm({...form, items:[...form.items, {productId:'', quantity:1, unitPrice:0}]})}>Add Item</button>
          {form.items.map((it:any, idx:number)=>(
            <div key={idx} className="inline-item">
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
    </div>
  );
}
