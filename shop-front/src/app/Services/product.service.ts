import { EventEmitter, Injectable } from '@angular/core';
import { SimpleHttpClientService } from './simple-http-client.service';
import { ProductInput, ProductOutput } from '../interfaces/product';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  changeProducts = new EventEmitter<void>();

  constructor(private http: SimpleHttpClientService) {}

  async getAllProducts() {
    return firstValueFrom(this.http.get<Array<ProductOutput>>('api/Product'));
  }

  async AddNewProduct(product: ProductInput) {
    return firstValueFrom(
      this.http.post<ProductOutput>('api/Product', product)
    );
  }

  async EditProduct(id: number, product: ProductInput) {
    return firstValueFrom(
      this.http.patch<ProductOutput>(`api/Product/${id}`, product)
    );
  }

  async DeleteProduct(id: number) {
    return firstValueFrom(this.http.delete<ProductOutput>(`api/Product/${id}`));
  }
}
