import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProductOutput } from '../../interfaces/product';
import { ProductService } from '../../Services/product.service';
import { ModalNavigateService } from '../../Services/modal-navigate.service';
import { AddNewProductComponent } from '../modal/add-new-product/add-new-product.component';
import { ShowProductInfoComponent } from '../modal/show-product-info/show-product-info.component';
import { EditProductComponent } from '../modal/edit-product/edit-product.component';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [],
  templateUrl: './products-list.component.html',
  styleUrl: './products-list.component.css',
})
export class ProductsListComponent implements OnInit {
  static Path = 'products-list';

  products: ProductOutput[] = [];
  isEdible = false;

  constructor(
    private productService: ProductService,
    private cdr: ChangeDetectorRef,
    private modal: ModalNavigateService,
    private authService: AuthenticationService
  ) {
    this.isEdible = this.authService.user?.role === 'Admin';
  }

  ngOnInit() {
    this.getAllCategories();
    this.productService.changeProducts.subscribe(() => {
      this.getAllCategories();
    });
  }

  async getAllCategories() {
    const response = await this.productService.getAllProducts();
    if (response.ok) {
      this.products = response.body!;
      this.cdr.detectChanges();
    }
  }

  addNewProduct() {
    this.modal.goToModal([AddNewProductComponent.Path]);
  }

  editProduct(product: ProductOutput, event: Event) {
    event.stopPropagation();
    this.modal.goToModal(
      [EditProductComponent.Path, product.id!.toString()],
      product
    );
  }

  showProduct(product: ProductOutput) {
    this.modal.goToModal(
      [ShowProductInfoComponent.Path, product.id!.toString()],
      product
    );
  }
}
