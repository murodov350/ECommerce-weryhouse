import { useEffect, useState } from 'react';
import { apiFetch, clearToken } from '../auth.ts';
import PermissionsManager from './PermissionsManager';

export default function AdminPage({ onLogout }: { onLogout: () => void }) {
  const [status, setStatus] = useState<string>('Loading...');

  useEffect(() => {
    apiFetch('/api/admin/ping').then(async r => {
      if (!r.ok) {
        setStatus('Not authorized');
      } else {
        const data = await r.json();
        setStatus(data.message);
      }
    });
  }, []);

  return (
    <div className="admin-layout">
      <aside className="admin-sidebar">
        <h3>Admin Panel</h3>
        <ul>
          <li>Status</li>
        </ul>
        <button onClick={() => { clearToken(); onLogout(); }}>Logout</button>
      </aside>
      <main className="admin-main">
        <h1>Admin Dashboard</h1>
        <p>{status}</p>
        <PermissionsManager />
      </main>
    </div>
  );
}
