export interface Category {
  Id?: number;
  Name: string;
  Standard: number;
  Price: number;
  IsPrimary: boolean;
}

export interface returnedCategory {
  id: number;
  name: string;
  standard: number;
  price: number;
  isPrimary: boolean;
}
