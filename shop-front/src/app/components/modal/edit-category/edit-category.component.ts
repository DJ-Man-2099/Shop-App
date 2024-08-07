import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../../Services/category.service';
import {
  FormKeys,
  EnhancedFormBuilderService,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { Location } from '@angular/common';
import { Category } from '../../../interfaces/category';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [ReactiveFormsModule],
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
    private currentLocation: Location
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
    if (category.IsPrimary) {
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
    console.log(this.editCategory);

    const response = await this.categoryService.editCategory(
      this.editCategory.Id!,
      this.editCategoryForm.value
    );
    if (response.ok) {
      this.categoryService.changeCateogryPrices.emit();
      this.onDismiss();
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
