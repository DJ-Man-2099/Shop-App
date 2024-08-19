import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Location } from '@angular/common';

import { CategoryService } from '../../../Services/category.service';
import {
  FormKeys,
  EnhancedFormBuilderService,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { Category, returnedCategory } from '../../../interfaces/category';
import { LoadingComponent } from '../../loading/loading.component';
import { MessageModalService } from '../../../Services/message-modal.service';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent],
  templateUrl: './edit-category.component.html',
  styleUrl: './edit-category.component.css',
})
export class EditCategoryComponent {
  static Path = 'EditCategory';

  editCategoryForm!: FormGroup;
  editCategoryFormControlNames!: FormKeys[];

  editCategory!: Category;

  isLoaded = false;

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private categoryService: CategoryService,
    private currentLocation: Location,
    private messageService: MessageModalService
  ) {}

  ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const { data: category } = this.currentLocation.getState() as {
      data: Category;
    };

    this.editCategory = category;
    console.log(category);

    this.isLoaded = true;

    const formConfig: any = {
      Name: {
        type: 'text',
        displayName: 'أسم العيار',
        controls: [category.Name, Validators.required],
      },
      Standard: {
        type: 'number',
        displayName: 'العيار',
        controls: [category.Standard, [Validators.required, Validators.min(0)]],
      },
    };

    // Conditionally add the Price field if the category is Primary
    if (category.Type === 'Primary') {
      formConfig.Price = {
        type: 'number',
        displayName: 'السعر',
        controls: [category.Price, [Validators.required, Validators.min(0)]],
      };
    }

    this.efb.createForm(formConfig);

    this.editCategoryForm = this.efb.resultForm;
    this.editCategoryFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    const response = await this.categoryService.editCategory(
      this.editCategory.Id!,
      this.editCategoryForm.value
    );
    if (response.ok) {
      this.categoryService.changeCateogryPrices.emit();
      this.onDismiss();
    }
  }

  async onDelete() {
    var res = await this.messageService.showConfirmMessage(
      'هل انت متأكد من رغبتك في الغاء العيار؟',
      'تأكيد'
    );
    if (res) {
      this.onAccept();
    }
  }

  async onAccept() {
    const response = await this.categoryService.deleteCategory(
      this.editCategory.Id!
    );
    if (response.ok) {
      this.categoryService.changeCateogryPrices.emit();
      this.onDismiss();
    }
  }

  async onChangeBase() {
    var res = await this.messageService.showConfirmMessage(
      'هل انت متأكد من اختيار هذا المعيار كأساسي؟',
      'تأكيد'
    );
    if (res) {
      const response = await this.categoryService.changeBaseCategory(
        this.editCategory.Id!
      );
      if (response.ok) {
        this.categoryService.changeBaseCategoryEvent.emit();
        this.onDismiss();
      }
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
