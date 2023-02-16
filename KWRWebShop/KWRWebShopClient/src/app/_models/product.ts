import { Category } from './category';

export interface Product {
    productId: number;
    name: string;
    brand: string;
    description: string;
    price: number;
    categoryId: number;
    category: {
        categoryId: number,
        categoryName: '',
    };
}
