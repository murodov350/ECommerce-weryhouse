import { useEffect, useState } from 'react';

type Column<T> = { key: keyof T | string; header: string; render?: (row: T) => any };

type Props<T> = {
  title: string;
  fetchItems: () => Promise<any>;
  columns: Column<T>[];
  onEdit?: (row: T) => void;
  onDelete?: (row: T) => void;
  actions?: (row: T) => any;
  toolbar?: any;
};

export function CrudTable<T extends { id: string | number }>({ title, fetchItems, columns, onEdit, onDelete, actions, toolbar }: Props<T>) {
  const [items, setItems] = useState<T[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      setLoading(true); setError('');
      try { setItems(await fetchItems()); } catch { setError('Load failed'); }
      finally { setLoading(false); }
    })();
  }, [fetchItems]);

  return (
    <div>
      <div style={{display:'flex', justifyContent:'space-between', alignItems:'center'}}>
        <h2>{title}</h2>
        {toolbar}
      </div>
      {loading ? <p>Loading...</p> : error ? <p className="error">{error}</p> : (
        <table className="table">
          <thead>
            <tr>
              {columns.map((c,i)=>(<th key={i}>{c.header}</th>))}
              {(onEdit || onDelete || actions) && <th>Actions</th>}
            </tr>
          </thead>
          <tbody>
            {items.map((row:any)=>(
              <tr key={row.id}>
                {columns.map((c,i)=> <td key={i}>{c.render? c.render(row): (row as any)[c.key as string]}</td>)}
                {(onEdit || onDelete || actions) && (
                  <td style={{display:'flex', gap:'.5rem'}}>
                    {actions && actions(row)}
                    {onEdit && <button className="btn primary" onClick={()=>onEdit(row)}>Edit</button>}
                    {onDelete && <button className="btn" onClick={()=>onDelete(row)}>Delete</button>}
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
