import { useState } from 'react'
import './App.css'
import LoginPage from './pages/LoginPage'
import HomePage from './pages/HomePage'
import AdminPage from './pages/AdminPage'
import CategoriesPage from './pages/CategoriesPage'
import SuppliersPage from './pages/SuppliersPage'
import ProductsPage from './pages/ProductsPage'
import OrdersPage from './pages/OrdersPage'
import StockPage from './pages/StockPage'
import { getToken, clearToken } from './auth.ts'
import Layout from './components/Layout'

function App() {
  const [authed, setAuthed] = useState(!!getToken())
  const [view, setView] = useState<'home'|'admin'|'categories'|'suppliers'|'products'|'orders'|'stock'>('home')

  if (!authed) return <LoginPage onLogin={() => { setAuthed(true); setView('home'); }} />

  const nav = [
    { key:'home', label:'Home', onClick:()=>setView('home') },
    { key:'admin', label:'Admin', onClick:()=>setView('admin') },
    { key:'categories', label:'Categories', onClick:()=>setView('categories') },
    { key:'suppliers', label:'Suppliers', onClick:()=>setView('suppliers') },
    { key:'products', label:'Products', onClick:()=>setView('products') },
    { key:'orders', label:'Orders', onClick:()=>setView('orders') },
    { key:'stock', label:'Stock', onClick:()=>setView('stock') },
  ];

  const title = ({home:'Home', admin:'Admin', categories:'Categories', suppliers:'Suppliers', products:'Products', orders:'Orders', stock:'Stock'} as any)[view];

  return (
    <Layout title={title} nav={nav} onLogout={()=>{ clearToken(); setAuthed(false); }}>
      {view==='home' && <HomePage onLogout={() => { clearToken(); setAuthed(false); }} goAdmin={() => setView('admin')} />}
      {view==='admin' && <AdminPage onLogout={() => { clearToken(); setAuthed(false); }} />}
      {view==='categories' && <CategoriesPage />}
      {view==='suppliers' && <SuppliersPage />}
      {view==='products' && <ProductsPage />}
      {view==='orders' && <OrdersPage />}
      {view==='stock' && <StockPage />}
    </Layout>
  );
}

export default App
