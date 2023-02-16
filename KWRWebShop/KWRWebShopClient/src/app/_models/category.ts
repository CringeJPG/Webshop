
import { Product } from "./product";

export interface Category {
  categoryId: number;
  categoryName: string;
  products: Product[];
}
