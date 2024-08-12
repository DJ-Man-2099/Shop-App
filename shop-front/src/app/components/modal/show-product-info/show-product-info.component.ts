import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ProductOutput } from '../../../interfaces/product';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';

@Component({
  selector: 'app-show-product-info',
  standalone: true,
  imports: [],
  templateUrl: './show-product-info.component.html',
  styleUrl: './show-product-info.component.css',
})
export class ShowProductInfoComponent implements OnInit {
  static Path = 'show-product-info';

  product!: ProductOutput;

  constructor(
    private modal: ModalNavigateService,
    private currentLocation: Location
  ) {}

  ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const { data: product } = this.currentLocation.getState() as {
      data: ProductOutput;
    };

    console.log(product);

    this.product = product;
  }
  onDismiss() {
    this.modal.dismiss();
  }
}
