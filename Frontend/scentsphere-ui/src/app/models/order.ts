export interface OrderItem {
  productName: string;

  quantity: number;

  unitPrice: number;

  totalPrice: number;
}

export interface Order {
  orderId: string;

  totalAmount: number;

  status: string;

  createdAt: string;

  items: OrderItem[];
}
