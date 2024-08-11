export interface GroupOutput {
  id: number;
  name: string;
  category: GroupCategory;
}

interface GroupCategory {
  id: number;
  name: string;
}

export interface GroupInput {
  name: string;
  categoryId: number;
}
