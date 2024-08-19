import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../../Services/enhanced-form-builder.service';
import { Location } from '@angular/common';

import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { GroupService } from '../../../Services/group.service';
import { ProductService } from '../../../Services/product.service';
import { LoadingComponent } from '../../loading/loading.component';
import { ProductOutput } from '../../../interfaces/product';
import { MessageModalService } from '../../../Services/message-modal.service';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css',
})
export class EditProductComponent implements OnInit {
  static Path = 'add-new-product';
  isLoading = true;

  newProductForm!: FormGroup;
  newProductFormControlNames!: FormKeys[];

  editProduct!: ProductOutput;

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private groupService: GroupService,
    private productService: ProductService,
    private currentLocation: Location,
    private messageService: MessageModalService
  ) {}

  async ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const { data: product } = this.currentLocation.getState() as {
      data: ProductOutput;
    };

    this.editProduct = product;
    var groups = await this.groupService.getAllGroups();
    if (!groups.ok) {
      console.log('error');
    }
    this.isLoading = false;
    var options = groups.body?.map((c) => ({
      label: c.name,
      value: c.id,
    }));
    this.efb.createForm({
      name: {
        type: 'text',
        displayName: 'أسم المنتج',
        controls: [this.editProduct.name, Validators.required],
      },
      manufacturePrice: {
        type: 'number',
        displayName: 'سعر التشغيلة',
        controls: [
          this.editProduct.manufacturePrice,
          [Validators.required, Validators.min(0)],
        ],
      },
      weightGrams: {
        type: 'number',
        displayName: 'الوزن بالجرامات',
        controls: [
          this.editProduct.weightGrams,
          [Validators.required, Validators.min(0)],
        ],
      },
      weightMilliGrams: {
        type: 'number',
        displayName: 'الوزن بالملليجرامات',
        controls: [
          this.editProduct.weightMilliGrams,
          [Validators.required, Validators.min(0)],
        ],
      },
      groupId: {
        type: 'select',
        displayName: 'المجموعة',
        options: options,
        controls: [
          this.editProduct.group.id,
          [Validators.required, Validators.min(0)],
        ],
      },
    });

    this.newProductForm = this.efb.resultForm;
    this.newProductFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    var res = await this.productService.EditProduct(
      this.editProduct.id,
      this.newProductForm.value
    );
    if (res.ok) {
      this.productService.changeProducts.emit();
      this.modal.dismiss();
    }
  }

  async onDelete() {
    var res = await this.messageService.showConfirmMessage(
      'هل انت متأكد من رغبتك في الغاء المنتج؟',
      'تأكيد'
    );
    if (res) {
      this.onAccept();
    }
  }

  async onAccept() {
    const response = await this.productService.DeleteProduct(
      this.editProduct.id!
    );
    if (response.ok) {
      this.productService.changeProducts.emit();
      this.onDismiss();
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
