import { useEffect, useState } from 'react';
import { apiFetch, clearToken } from '../auth.ts';

interface Role { id: string; name: string; }
interface Permission { id: string; module: string; action: string; }

export default function HomePage({ onLogout, goAdmin }: { onLogout: () => void; goAdmin: () => void }) {
  const [roles, setRoles] = useState<Role[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);

  useEffect(() => {
    apiFetch('/api/roles').then(async r => { if (r.ok) setRoles(await r.json()); });
    apiFetch('/api/permissions/catalog').then(async r => { if (r.ok) setPermissions(await r.json()); });
  }, []);

  return (
    <div className="admin-layout">
      <aside className="admin-sidebar">
        <h3>Menu</h3>
        <ul>
          <li><button className="btn primary" onClick={goAdmin}>Admin Panel</button></li>
          <li><button className="btn muted" onClick={() => { clearToken(); onLogout(); }}>Logout</button></li>
        </ul>
      </aside>
      <main className="admin-main">
        <h1>Home</h1>
        <section>
          <h2>Roles</h2>
          <table className="table">
            <thead><tr><th>Name</th></tr></thead>
            <tbody>{roles.map(r => <tr key={r.id}><td>{r.name}</td></tr>)}</tbody>
          </table>
        </section>
        <section>
          <h2>Permissions</h2>
          <table className="table">
            <thead><tr><th>Module</th><th>Action</th></tr></thead>
            <tbody>{permissions.map(p => <tr key={p.id}><td>{p.module}</td><td>{p.action}</td></tr>)}</tbody>
          </table>
        </section>
      </main>
    </div>
  );
}
