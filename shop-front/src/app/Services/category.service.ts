import { EventEmitter, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Category, returnedCategory } from '../interfaces/category';
import { SimpleHttpClientService } from './simple-http-client.service';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private http: SimpleHttpClientService) {}

  changeCateogryPrices = new EventEmitter<void>();
  changeBaseCategoryEvent = new EventEmitter<void>();

  getBaseCategory(): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(this.http.get<returnedCategory>('api/Category/base'));
  }

  getAllCategories(): Promise<HttpResponse<returnedCategory[]>> {
    return firstValueFrom(this.http.get<returnedCategory[]>('api/Category'));
  }

  changeBaseCategoryPrice(
    price: number
  ): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(
      this.http.patch<returnedCategory>('api/Category/base', { price })
    );
  }

  addCategory(baseCategory: Category): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(
      this.http.post<returnedCategory>('api/Category', baseCategory)
    );
  }

  editCategory(
    id: number,
    baseCategory: Category
  ): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(
      this.http.patch<returnedCategory>(`api/Category/${id}`, baseCategory)
    );
  }

  deleteCategory(id: number): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(
      this.http.delete<returnedCategory>(`api/Category/${id}`)
    );
  }

  changeBaseCategory(id: number): Promise<HttpResponse<returnedCategory>> {
    return firstValueFrom(
      this.http.post<returnedCategory>(`api/Category/changebase/${id}`, null)
    );
  }
}
