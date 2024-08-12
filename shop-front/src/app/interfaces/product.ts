import { GroupOutput } from './group';

export interface ProductInput {
  name: string;
  manufacturePrice: number;
  weightGrams: number;
  weightMilliGrams: number;
  groupId: number;
}

export interface ProductOutput {
  id: number;
  name: string;
  manufacturePrice: number;
  weightGrams: number;
  weightMilliGrams: number;
  group: GroupOutput;
}
