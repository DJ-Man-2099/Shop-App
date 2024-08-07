import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, firstValueFrom, of } from 'rxjs';

import { Category, returnedCategory } from '../interfaces/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private http: HttpClient) {}

  changeCateogryPrices = new EventEmitter<void>();
  changeBaseCategoryEvent = new EventEmitter<void>();

  getBaseCategory() {
    return firstValueFrom(
      this.http
        .get<returnedCategory>('api/Category/base', {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  getAllCategories() {
    return firstValueFrom(
      this.http
        .get<returnedCategory[]>('api/Category', {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  changeBaseCategoryPrice(price: number) {
    return firstValueFrom(
      this.http
        .patch<returnedCategory>(
          'api/Category/base',
          { price },
          {
            observe: 'response',
          }
        )
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  addCategory(baseCategory: Category) {
    return firstValueFrom(
      this.http
        .post<returnedCategory>('api/Category', baseCategory, {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  editCategory(id: number, baseCategory: Category) {
    return firstValueFrom(
      this.http
        .patch<returnedCategory>(`api/Category/${id}`, baseCategory, {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  deleteCategory(id: number) {
    return firstValueFrom(
      this.http
        .delete<returnedCategory>(`api/Category/${id}`, {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }

  changeBaseCategory(id: number) {
    return firstValueFrom(
      this.http
        .post<returnedCategory>(`api/Category/changebase/${id}`, null, {
          observe: 'response',
        })
        .pipe(
          catchError((err) =>
            of({
              ok: false,
              error: err,
              body: undefined,
            })
          )
        )
    );
  }
}
