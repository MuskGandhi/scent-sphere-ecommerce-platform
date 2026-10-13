export interface Brand {
  id: string;

  name: string;
}

export interface Category {
  id: string;

  name: string;
}

export interface AdminProduct {
  id: string;

  name: string;

  description: string;

  price: number;

  stockQuantity: number;

  isFeatured: boolean;

  brandId: string;

  categoryId: string;
}

export interface CreateProductRequest {
  name: string;

  description: string;

  price: number;

  stockQuantity: number;

  isFeatured: boolean;

  brandId: string;

  categoryId: string;
}

export interface UpdateProductRequest {
  name: string;

  description: string;

  price: number;

  stockQuantity: number;

  isFeatured: boolean;

  brandId: string;

  categoryId: string;
}

export interface AdminProduct {
  id: string;

  name: string;

  description: string;

  price: number;

  stockQuantity: number;

  isFeatured: boolean;

  brandId: string;

  categoryId: string;
}
