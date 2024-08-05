import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

import { BaseCategoryInfo } from '../interfaces/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private http: HttpClient) {}

  getBaseCategory() {
    return this.http.get<BaseCategoryInfo>('api/Category/base', {
      observe: 'response',
    });
  }

  changeBaseCategoryPrice(price: number) {
    return firstValueFrom(
      this.http.patch<BaseCategoryInfo>(
        'api/Category/base',
        { price },
        {
          observe: 'response',
        }
      )
    );
  }
}
