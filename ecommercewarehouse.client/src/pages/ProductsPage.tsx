import { useCallback, useEffect, useState } from 'react';
import { getProducts, createProduct, updateProduct, deleteProduct, getCategories } from '../components/api';

export default function ProductsPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', sku:'', price:0, quantity:0, categoryId:'' });
  const [categories,setCategories]=useState<any[]>([]);
  const [items,setItems]=useState<any[]>([]);
  const [search,setSearch]=useState('');
  const [pageNumber,setPageNumber]=useState(1); const pageSize=10;
  const [totalPages,setTotalPages]=useState(1);
  const [sort,setSort]=useState(''); const [desc,setDesc]=useState(false);

  useEffect(()=>{ getCategories().then((c:any)=> setCategories(c.items||c)); },[]);

  const load = useCallback(async ()=>{
    const data = await getProducts({ search, pageNumber, pageSize, sort: sort||undefined, desc });
    if(data.items){ setItems(data.items); setTotalPages(data.totalPages); } else { setItems(data); setTotalPages(1); }
  },[search,pageNumber,sort,desc]);

  useEffect(()=>{ load(); },[load]);

  const openCreate = ()=>{ setEditing(null); setForm({name:'',sku:'',price:0,quantity:0,categoryId:''}); const modal=document.getElementById('prodModal')!; new (window as any).bootstrap.Modal(modal).show(); };
  const onEdit = (row:any)=>{ setEditing(row); setForm({ name: row.name, sku: row.sku, price: row.price, quantity: row.quantity, categoryId: row.categoryId }); const modal=document.getElementById('prodModal')!; new (window as any).bootstrap.Modal(modal).show(); };
  const onDelete = async (row:any)=>{ await deleteProduct(row.id); load(); };

  const onSave = async ()=>{
    const payload = { ...form, price:Number(form.price), quantity:Number(form.quantity) };
    if(editing){ await updateProduct(editing.id, payload); } else { await createProduct(payload); }
    (window as any).bootstrap.Modal.getInstance(document.getElementById('prodModal')!)?.hide();
    load();
  };

  return (
    <div className="container-fluid">
      <div className="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
        <h2 className="mb-0">Products</h2>
        <div className="d-flex flex-wrap gap-2">
          <input placeholder="Search" className="form-control" value={search} onChange={e=>{ setSearch(e.target.value); setPageNumber(1); }} />
          <select className="form-select" value={sort} onChange={e=>{ setSort(e.target.value); setPageNumber(1); }}>
            <option value="">Sort</option>
            <option value="name">Name</option>
            <option value="price">Price</option>
            <option value="quantity">Quantity</option>
          </select>
          <select className="form-select" value={desc? 'desc':'asc'} onChange={e=>{ setDesc(e.target.value==='desc'); }}>
            <option value="asc">Asc</option>
            <option value="desc">Desc</option>
          </select>
          <button className="btn btn-primary" onClick={openCreate}><i className="fa fa-plus"/> New</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-striped table-hover">
          <thead><tr><th>Name</th><th>SKU</th><th>Price</th><th>Qty</th><th>Category</th><th style={{width:160}}>Actions</th></tr></thead>
          <tbody>
            {items.map(p=> (
              <tr key={p.id}>
                <td>{p.name}</td>
                <td>{p.sku}</td>
                <td>{p.price}</td>
                <td>{p.quantity}</td>
                <td>{p.categoryName}</td>
                <td>
                  <button className="btn btn-sm btn-secondary me-2" onClick={()=>onEdit(p)}><i className="fa fa-edit"/></button>
                  <button className="btn btn-sm btn-outline-danger" onClick={()=>onDelete(p)}><i className="fa fa-trash"/></button>
                </td>
              </tr>
            ))}
            {items.length===0 && <tr><td colSpan={6} className="text-center text-muted">No data</td></tr>}
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

      <div className="modal fade" id="prodModal" tabIndex={-1} aria-hidden="true">
        <div className="modal-dialog modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{editing? 'Edit':'Create'} Product</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="row g-3">
                <div className="col-md-6"><label className="form-label">Name</label><input className="form-control" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/></div>
                <div className="col-md-6"><label className="form-label">SKU</label><input className="form-control" value={form.sku} onChange={e=>setForm({...form, sku:e.target.value})}/></div>
                <div className="col-md-4"><label className="form-label">Price</label><input type="number" className="form-control" value={form.price} onChange={e=>setForm({...form, price:e.target.value as any})}/></div>
                <div className="col-md-4"><label className="form-label">Quantity</label><input type="number" className="form-control" value={form.quantity} onChange={e=>setForm({...form, quantity:e.target.value as any})}/></div>
                <div className="col-md-4"><label className="form-label">Category</label><select className="form-select" value={form.categoryId} onChange={e=>setForm({...form, categoryId:e.target.value})}>
                  <option value="">-- Category --</option>
                  {categories.map(c=> <option key={c.id} value={c.id}>{c.name}</option>)}
                </select></div>
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
