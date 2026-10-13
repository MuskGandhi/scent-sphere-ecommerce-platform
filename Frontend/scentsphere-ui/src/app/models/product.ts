export interface ProductImage {
  imageUrl: string;
  displayOrder: number;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  brand: string;
  category: string;
  images: ProductImage[];
}
