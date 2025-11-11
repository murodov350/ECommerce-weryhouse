import { useCallback, useEffect, useState } from 'react';
import { CrudTable } from '../components/CrudTable';
import { createStockTx, getProducts, getStock } from '../components/api';

export default function StockPage(){
  const [form,setForm]=useState({ productId:'', quantity:0, type:'In' });
  const [products,setProducts]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [refreshKey,setRefreshKey]=useState(0);

  useEffect(()=>{ getProducts().then((p:any)=> setProducts(p.items||p)); },[]);

  const load = useCallback(async ()=>{ const data = await getStock(); setItems(data.items||data); },[]);
  useEffect(()=>{ load(); },[refreshKey, load]);

  const onSave = async ()=>{ await createStockTx({...form, quantity:Number(form.quantity)}); setForm({productId:'', quantity:0, type:'In'}); setRefreshKey(k=>k+1); };

  return (
    <div>
      <CrudTable title="Stock Transactions" fetchItems={async()=>items}
        columns={[{key:'productName', header:'Product'},{key:'type', header:'Type'},{key:'quantity', header:'Qty'},{key:'date', header:'Date'}]}
        toolbar={<></>}
      />
      <div className="card-form">
        <h3>New Transaction</h3>
        <div className="form-grid">
          <select value={form.productId} onChange={e=>setForm({...form, productId:e.target.value})}>
            <option value="">-- Product --</option>
            {products.map(p=> <option key={p.id} value={p.id}>{p.name}</option>)}
          </select>
          <select value={form.type} onChange={e=>setForm({...form, type:e.target.value})}>
            {['In','Out'].map(t=> <option key={t} value={t}>{t}</option>)}
          </select>
          <input type="number" placeholder="Quantity" value={form.quantity} onChange={e=>setForm({...form, quantity:e.target.value as any})}/>
          <button className="btn primary" onClick={onSave}>Save</button>
        </div>
      </div>
    </div>
  );
}
