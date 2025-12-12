export interface Book {
  id: string;
  title: string;
  isbn: string;
  description?: string;
  purchasePrice: number;
  rentalPricePerDay: number;
  stockPurchase: number;
  stockRental: number;
  imageUrl?: string;
  publishedDate?: string;
  authorId: string;
  categoryId: string;
}

export interface BookDTO {
  id: string;
  title: string;
  isbn: string;
  description?: string;
  purchasePrice: number;
  rentalPricePerDay: number;
  stockPurchase: number;
  stockRental: number;
  imageUrl?: string;
  authorId: string;
  authorName: string;
  categoryId: string;
  categoryName: string;
}
export interface BookDetailsDTO extends BookDTO {
    category : string | { id: string; name: string };
    author : string | { id: string; fullName: string; firstName?: string; lastName?: string };
}
export interface CreateBookDTO {
  title: string;
  isbn: string;
  description?: string;
  purchasePrice: number;
  rentalPricePerDay: number;
  stockPurchase: number;
  stockRental: number;
  imageUrl?: string;
  publishedDate?: string;
  authorId: string;
  categoryId: string;
}
export interface UpdateBookDTO {
  title?: string;
  isbn?: string;
  description?: string;
  purchasePrice?: number;
  rentalPricePerDay?: number;
  stockPurchase?: number;
  stockRental?: number;
  imageUrl?: string;
  publishedDate?: string;
  authorId?: string;
  categoryId?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
}

export type BooksListResponse = ApiResponse<BookDTO[]>;
export type BookResponse = ApiResponse<BookDTO>;
