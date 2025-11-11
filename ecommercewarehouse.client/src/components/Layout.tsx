import { ReactNode, useState } from 'react';

type NavItem = { key: string; label: string; onClick: () => void };

export default function Layout({ title, nav, children, onLogout }: { title: string; nav: NavItem[]; children: ReactNode; onLogout: () => void }) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  return (
    <div className="app-shell">
      <header className="topbar">
        <button className="burger" aria-label="Toggle navigation" onClick={()=>setSidebarOpen(!sidebarOpen)}>?</button>
        <div className="brand">Ecommerce Warehouse</div>
        <div className="spacer" />
        <div className="actions">
          <button className="btn muted" onClick={onLogout}>Logout</button>
        </div>
      </header>
      <div className="shell-body">
        <aside className={`sidebar ${sidebarOpen? 'open':''}`} onClick={()=>setSidebarOpen(false)}>
          <nav>
            {nav.map(n => (
              <button key={n.key} className="nav-link" onClick={n.onClick}>{n.label}</button>
            ))}
          </nav>
        </aside>
        <main className="content">
          <div className="page-header"><h1>{title}</h1></div>
          <div className="page-body">{children}</div>
        </main>
      </div>
    </div>
  );
}
