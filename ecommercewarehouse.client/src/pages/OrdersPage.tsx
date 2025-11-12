import { useCallback, useEffect, useState } from 'react';
import { createOrder, getOrders, getProducts, updateOrderStatus } from '../components/api';

export default function OrdersPage(){
  const [form,setForm]=useState({ customerName:'', items:[] as any[] });
  const [products,setProducts]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [search,setSearch]=useState('');
  const [pageNumber,setPageNumber]=useState(1); const pageSize=10; const [totalPages,setTotalPages]=useState(1);

  useEffect(()=>{ getProducts().then((p:any)=> setProducts(p.items||p)); },[]);

  const load = useCallback(async ()=>{ const data = await getOrders(); const list = data.items||data; // orders endpoint not paged yet
    const filtered = search? list.filter((o:any)=> o.orderNumber.includes(search)|| o.customerName.toLowerCase().includes(search.toLowerCase())): list;
    setTotalPages(Math.ceil(filtered.length / pageSize));
    setItems(filtered.slice((pageNumber-1)*pageSize, pageNumber*pageSize));
  },[search,pageNumber]);
  useEffect(()=>{ load(); },[load]);

  const setItem = (idx:number, patch:any)=>{
    const its = [...form.items]; its[idx] = { ...(its[idx]||{productId:'', quantity:1, unitPrice:0}), ...patch }; setForm({...form, items:its});
  };

  const addItem = ()=> setForm({...form, items:[...form.items, {productId:'', quantity:1, unitPrice:0}]});
  const removeItem = (idx:number)=> setForm({...form, items: form.items.filter((_,i)=> i!==idx)});

  const onSave = async ()=>{ await createOrder(form); setForm({customerName:'', items:[]}); load(); (window as any).bootstrap?.Modal.getInstance(document.getElementById('orderModal')!)?.hide(); };

  const openCreate = ()=>{ setForm({customerName:'', items:[]}); const modal=document.getElementById('orderModal')!; new (window as any).bootstrap.Modal(modal).show(); };

  return (
    <div className="container-fluid">
      <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
        <h2 className="mb-0">Orders</h2>
        <div className="d-flex gap-2">
          <input placeholder="Search" className="form-control" value={search} onChange={e=>{ setSearch(e.target.value); setPageNumber(1); }} />
          <button className="btn btn-primary" onClick={openCreate}><i className="fa fa-plus"/> New</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-striped table-hover">
          <thead><tr><th>Order #</th><th>Customer</th><th>Total</th><th>Status</th></tr></thead>
          <tbody>
            {items.map(o=> (
              <tr key={o.id}>
                <td>{o.orderNumber}</td>
                <td>{o.customerName}</td>
                <td>{o.totalAmount}</td>
                <td>
                  <select className="form-select form-select-sm" defaultValue={o.status} onChange={async e=>{ await updateOrderStatus(o.id, e.target.value); load(); }}>
                    {['Pending','Paid','Shipped','Cancelled'].map(s=> <option key={s} value={s}>{s}</option>)}
                  </select>
                </td>
              </tr>
            ))}
            {items.length===0 && <tr><td colSpan={4} className="text-center text-muted">No data</td></tr>}
          </tbody>
        </table>
      </div>
      {totalPages>1 && (
        <nav>
          <ul className="pagination pagination-sm">
            <li className={`page-item ${pageNumber===1?'disabled':''}`}><button className="page-link" onClick={()=> pageNumber>1 && setPageNumber(p=>p-1)}>Prev</button></li>
            <li className="page-item disabled"><span className="page-link">{pageNumber}/{totalPages}</span></li>
            <li className={`page-item ${pageNumber===totalPages?'disabled':''}`}><button className="page-link" onClick={()=> pageNumber<totalPages && setPageNumber(p=>p+1)}>Next</button></li>
          </ul>
        </nav>
      )}

      <div className="modal fade" id="orderModal" tabIndex={-1} aria-hidden="true">
        <div className="modal-dialog modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Create Order</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="mb-3"><label className="form-label">Customer Name</label><input className="form-control" value={form.customerName} onChange={e=>setForm({...form, customerName:e.target.value})}/></div>
              <div className="mb-2 d-flex justify-content-between align-items-center">
                <strong>Items</strong>
                <button className="btn btn-sm btn-outline-primary" onClick={addItem}><i className="fa fa-plus"/> Item</button>
              </div>
              {form.items.map((it:any, idx:number)=>(
                <div key={idx} className="row g-2 align-items-end mb-2">
                  <div className="col-md-5">
                    <label className="form-label">Product</label>
                    <select className="form-select" value={it.productId} onChange={e=> setItem(idx, { productId:e.target.value })}>
                      <option value="">-- Product --</option>
                      {products.map(p=> <option key={p.id} value={p.id}>{p.name}</option>)}
                    </select>
                  </div>
                  <div className="col-md-2">
                    <label className="form-label">Qty</label>
                    <input type="number" className="form-control" value={it.quantity} onChange={e=> setItem(idx, { quantity:Number(e.target.value) })} />
                  </div>
                  <div className="col-md-3">
                    <label className="form-label">Unit Price</label>
                    <input type="number" className="form-control" value={it.unitPrice} onChange={e=> setItem(idx, { unitPrice:Number(e.target.value) })} />
                  </div>
                  <div className="col-md-2 d-flex">
                    <button className="btn btn-outline-danger ms-auto" onClick={()=>removeItem(idx)}><i className="fa fa-times"/></button>
                  </div>
                </div>
              ))}
              {form.items.length===0 && <div className="text-muted">No items added yet.</div>}
            </div>
            <div className="modal-footer">
              <button className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button className="btn btn-primary" onClick={onSave}>Save Order</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
