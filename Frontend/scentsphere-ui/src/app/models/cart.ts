export interface Cart {
  cartTotal: number;

  items: CartItem[];
}

export interface CartItem {
  cartItemId: string;

  productId: string;

  productName: string;

  unitPrice: number;

  quantity: number;

  totalPrice: number;
}
