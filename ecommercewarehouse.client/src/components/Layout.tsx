import { ReactNode, useState, useEffect } from 'react';

interface NavItem { key: string; label: string; icon?: string; onClick: () => void }

export default function Layout({ title, nav, children, onLogout }: { title: string; nav: NavItem[]; children: ReactNode; onLogout: () => void }) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  useEffect(()=>{ const handler=()=> setSidebarOpen(false); window.addEventListener('hashchange',handler); return ()=> window.removeEventListener('hashchange',handler); },[]);
  return (
    <div className="app-shell full-height">
      <header className="topbar shadow-sm">
        <button className="burger" aria-label="Toggle navigation" onClick={()=>setSidebarOpen(!sidebarOpen)}><i className="fa fa-bars"/></button>
        <div className="brand d-flex align-items-center gap-2">
          <img src="/logo.svg" alt="logo" style={{height:32}} />
          <span className="fw-bold">Warehouse Admin</span>
        </div>
        <div className="spacer" />
        <div className="actions d-flex align-items-center gap-2">
          <button className="btn btn-sm btn-outline-light" onClick={onLogout}><i className="fa fa-sign-out-alt"/> Logout</button>
        </div>
      </header>
      <div className="shell-body">
        <aside className={`sidebar ${sidebarOpen? 'open':''}`}>
          <div className="sidebar-scroll">
            <nav className="nav flex-column">
              {nav.map(n => (
                <button key={n.key} className="nav-link text-start" onClick={n.onClick}>
                  {n.icon && <i className={`fa fa-${n.icon} me-2`}/>} {n.label}
                </button>
              ))}
            </nav>
          </div>
        </aside>
        <main className="content">
          <div className="page-header d-flex align-items-center justify-content-between flex-wrap gap-2">
            <h1 className="h4 mb-0">{title}</h1>
          </div>
          <div className="page-body">{children}</div>
        </main>
      </div>
    </div>
  );
}
