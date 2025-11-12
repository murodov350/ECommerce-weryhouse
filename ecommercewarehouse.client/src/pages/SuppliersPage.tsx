import { useCallback, useEffect, useState } from 'react';
import { getSuppliers, createSupplier, updateSupplier, deleteSupplier } from '../components/api';

export default function SuppliersPage(){
  const [editing,setEditing]=useState<any|null>(null);
  const [form,setForm]=useState({ name:'', phone:'', email:'' });
  const [items,setItems]=useState<any[]>([]);
  const [search,setSearch]=useState('');
  const [pageNumber,setPageNumber]=useState(1); const pageSize=10;
  const [totalPages,setTotalPages]=useState(1);

  const load = useCallback(async ()=>{
    const data = await getSuppliers({ search, pageNumber, pageSize });
    if(data.items){ setItems(data.items); setTotalPages(data.totalPages); } else { setItems(data); setTotalPages(1); }
  },[search,pageNumber]);

  useEffect(()=>{ load(); },[load]);

  const openCreate = ()=>{ setEditing(null); setForm({name:'',phone:'',email:''}); const modal=document.getElementById('supModal')!; new (window as any).bootstrap.Modal(modal).show(); };
  const onEdit = (row:any)=>{ setEditing(row); setForm({name:row.name, phone: row.phone||'', email: row.email||''}); const modal=document.getElementById('supModal')!; new (window as any).bootstrap.Modal(modal).show(); };
  const onDelete = async (row:any)=>{ await deleteSupplier(row.id); load(); };

  const onSave = async ()=>{
    if(editing){ await updateSupplier(editing.id, form); } else { await createSupplier(form); }
    (window as any).bootstrap.Modal.getInstance(document.getElementById('supModal')!)?.hide();
    load();
  };

  return (
    <div className="container-fluid">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2 className="mb-0">Suppliers</h2>
        <div className="d-flex gap-2">
          <input placeholder="Search" className="form-control" value={search} onChange={e=>{ setSearch(e.target.value); setPageNumber(1); }} />
          <button className="btn btn-primary" onClick={openCreate}><i className="fa fa-plus"/> New</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-striped table-hover">
          <thead><tr><th>Name</th><th>Phone</th><th>Email</th><th style={{width:140}}>Actions</th></tr></thead>
          <tbody>
            {items.map(s=> (
              <tr key={s.id}>
                <td>{s.name}</td>
                <td>{s.phone}</td>
                <td>{s.email}</td>
                <td>
                  <button className="btn btn-sm btn-secondary me-2" onClick={()=>onEdit(s)}><i className="fa fa-edit"/></button>
                  <button className="btn btn-sm btn-outline-danger" onClick={()=>onDelete(s)}><i className="fa fa-trash"/></button>
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

      <div className="modal fade" id="supModal" tabIndex={-1} aria-hidden="true">
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{editing? 'Edit':'Create'} Supplier</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="mb-3"><label className="form-label">Name</label><input className="form-control" value={form.name} onChange={e=>setForm({...form, name:e.target.value})}/></div>
              <div className="mb-3"><label className="form-label">Phone</label><input className="form-control" value={form.phone} onChange={e=>setForm({...form, phone:e.target.value})}/></div>
              <div className="mb-3"><label className="form-label">Email</label><input className="form-control" value={form.email} onChange={e=>setForm({...form, email:e.target.value})}/></div>
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
