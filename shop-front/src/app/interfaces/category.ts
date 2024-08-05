export interface BaseCategoryInfo {
  standard: number;
  price: number;
}

export type FormKeys = {
  key: string;
  type: 'text' | 'number';
  name: string;
};

export interface Category {
	Name: string;
	Standard: number;
	Price: number;
}
