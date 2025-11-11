import { apiFetch } from '../auth';

export async function getCategories(q?: string) {
  return apiFetch(`/api/categories${q ? '?' + q : ''}`).then(r => r.json());
}
export async function createCategory(data: { name: string; description?: string }) {
  return apiFetch('/api/categories', { method: 'POST', body: JSON.stringify(data) }).then(r => r.json());
}
export async function updateCategory(id: string, data: { name: string; description?: string }) {
  return apiFetch(`/api/categories/${id}`, { method: 'PUT', body: JSON.stringify(data) });
}
export async function deleteCategory(id: string) {
  return apiFetch(`/api/categories/${id}`, { method: 'DELETE' });
}

export async function getSuppliers(q?: string) {
  return apiFetch(`/api/suppliers${q ? '?' + q : ''}`).then(r => r.json());
}
export async function createSupplier(data: { name: string; phone?: string; email?: string }) {
  return apiFetch('/api/suppliers', { method: 'POST', body: JSON.stringify(data) }).then(r => r.json());
}
export async function updateSupplier(id: string, data: { name: string; phone?: string; email?: string }) {
  return apiFetch(`/api/suppliers/${id}`, { method: 'PUT', body: JSON.stringify(data) });
}
export async function deleteSupplier(id: string) {
  return apiFetch(`/api/suppliers/${id}`, { method: 'DELETE' });
}

export async function getProducts(q?: string) {
  return apiFetch(`/api/products${q ? '?' + q : ''}`).then(r => r.json());
}
export async function createProduct(data: { name: string; sku: string; price: number; quantity: number; categoryId: string }) {
  return apiFetch('/api/products', { method: 'POST', body: JSON.stringify(data) }).then(r => r.json());
}
export async function updateProduct(id: string, data: { name: string; sku: string; price: number; quantity: number; categoryId: string }) {
  return apiFetch(`/api/products/${id}`, { method: 'PUT', body: JSON.stringify(data) });
}
export async function deleteProduct(id: string) {
  return apiFetch(`/api/products/${id}`, { method: 'DELETE' });
}

export async function getOrders() {
  return apiFetch('/api/orders').then(r => r.json());
}
export async function createOrder(data: { customerName: string; items: { productId: string; quantity: number; unitPrice: number }[] }) {
  return apiFetch('/api/orders', { method: 'POST', body: JSON.stringify(data) }).then(r => r.json());
}
export async function updateOrderStatus(id: string, status: string) {
  return apiFetch(`/api/orders/${id}/status`, { method: 'PATCH', body: JSON.stringify({ status }) });
}

export async function getStock() {
  return apiFetch('/api/stock').then(r => r.json());
}
export async function createStockTx(data: { productId: string; quantity: number; type: string }) {
  return apiFetch('/api/stock', { method: 'POST', body: JSON.stringify(data) }).then(r => r.json());
}
