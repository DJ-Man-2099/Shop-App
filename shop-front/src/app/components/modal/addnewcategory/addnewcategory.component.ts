import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { CategoryService } from '../../../Services/category.service';

@Component({
  selector: 'app-addnewcategory',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './addnewcategory.component.html',
  styleUrl: './addnewcategory.component.css',
})
export class AddnewcategoryComponent implements OnInit {
  static Path = 'addNewCategory';

  newCategoryForm!: FormGroup;
  newCategoryFormControlNames!: FormKeys[];

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.efb.createForm({
      Name: {
        type: 'text',
        displayName: 'أسم العيار',
        controls: ['', Validators.required],
      },
      Standard: {
        type: 'number',
        displayName: 'العيار',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
    });

    this.newCategoryForm = this.efb.resultForm;
    this.newCategoryFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    const response = await this.categoryService.addCategory(
      this.newCategoryForm.value
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
