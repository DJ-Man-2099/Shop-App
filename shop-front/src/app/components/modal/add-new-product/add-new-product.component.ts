import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { GroupService } from '../../../Services/group.service';
import { ProductService } from '../../../Services/product.service';
import { LoadingComponent } from '../../loading/loading.component';

@Component({
  selector: 'app-add-new-product',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent],
  templateUrl: './add-new-product.component.html',
  styleUrl: './add-new-product.component.css',
})
export class AddNewProductComponent implements OnInit {
  static Path = 'add-new-product';
  isLoading = true;

  newProductForm!: FormGroup;
  newProductFormControlNames!: FormKeys[];

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private groupService: GroupService,
    private productService: ProductService
  ) {}

  async ngOnInit() {
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
        controls: ['', Validators.required],
      },
      manufacturePrice: {
        type: 'number',
        displayName: 'سعر التشغيلة',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
      weightGrams: {
        type: 'number',
        displayName: 'الوزن بالجرامات',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
      weightMilliGrams: {
        type: 'number',
        displayName: 'الوزن بالملليجرامات',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
      groupId: {
        type: 'select',
        displayName: 'المجموعة',
        options: options,
        controls: [options![0].value, [Validators.required, Validators.min(0)]],
      },
    });

    this.newProductForm = this.efb.resultForm;
    this.newProductFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    var res = await this.productService.AddNewProduct(
      this.newProductForm.value
    );
    if (res.ok) {
      this.productService.changeProducts.emit();
      this.modal.dismiss();
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
