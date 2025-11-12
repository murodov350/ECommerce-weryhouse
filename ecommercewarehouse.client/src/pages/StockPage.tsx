import { useCallback, useEffect, useState } from 'react';
import { createStockTx, getProducts, getStock } from '../components/api';

export default function StockPage(){
  const [form,setForm]=useState({ productId:'', quantity:0, type:'In' });
  const [products,setProducts]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [search,setSearch]=useState('');
  const [pageNumber,setPageNumber]=useState(1); const pageSize=10; const [totalPages,setTotalPages]=useState(1);

  useEffect(()=>{ getProducts().then((p:any)=> setProducts(p.items||p)); },[]);

  const load = useCallback(async ()=>{ const data = await getStock(); const list = data.items||data;
    const filtered = search? list.filter((x:any)=> x.productName.toLowerCase().includes(search.toLowerCase()) || x.type.toLowerCase().includes(search.toLowerCase())): list;
    setTotalPages(Math.ceil(filtered.length / pageSize)); setItems(filtered.slice((pageNumber-1)*pageSize, pageNumber*pageSize));
  },[search,pageNumber]);
  useEffect(()=>{ load(); },[load]);

  const onSave = async ()=>{ await createStockTx({...form, quantity:Number(form.quantity)}); setForm({productId:'', quantity:0, type:'In'}); load(); (window as any).bootstrap?.Modal.getInstance(document.getElementById('stockModal')!)?.hide(); };

  const openCreate = ()=>{ setForm({productId:'', quantity:0, type:'In'}); const modal=document.getElementById('stockModal')!; new (window as any).bootstrap.Modal(modal).show(); };

  return (
    <div className="container-fluid">
      <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
        <h2 className="mb-0">Stock</h2>
        <div className="d-flex gap-2">
          <input placeholder="Search" className="form-control" value={search} onChange={e=>{ setSearch(e.target.value); setPageNumber(1); }} />
          <button className="btn btn-primary" onClick={openCreate}><i className="fa fa-plus"/> New</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-striped table-hover">
          <thead><tr><th>Product</th><th>Type</th><th>Qty</th><th>Date</th></tr></thead>
          <tbody>
            {items.map(t=> (
              <tr key={t.id}>
                <td>{t.productName}</td>
                <td>{t.type}</td>
                <td>{t.quantity}</td>
                <td>{new Date(t.date).toLocaleString()}</td>
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

      <div className="modal fade" id="stockModal" tabIndex={-1} aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">New Transaction</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="mb-3"><label className="form-label">Product</label>
                <select className="form-select" value={form.productId} onChange={e=>setForm({...form, productId:e.target.value})}>
                  <option value="">-- Product --</option>
                  {products.map(p=> <option key={p.id} value={p.id}>{p.name}</option>)}
                </select>
              </div>
              <div className="mb-3"><label className="form-label">Type</label>
                <select className="form-select" value={form.type} onChange={e=>setForm({...form, type:e.target.value})}>
                  {['In','Out'].map(t=> <option key={t} value={t}>{t}</option>)}
                </select>
              </div>
              <div className="mb-3"><label className="form-label">Quantity</label>
                <input type="number" className="form-control" value={form.quantity} onChange={e=>setForm({...form, quantity:e.target.value as any})}/>
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button className="btn btn-primary" onClick={onSave}>Save</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
