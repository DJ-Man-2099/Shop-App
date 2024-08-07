import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { CategoryService } from '../../Services/category.service';
import { returnedCategory } from '../../interfaces/category';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../Services/enhanced-form-builder.service';
import { LoadingComponent } from '../loading/loading.component';

@Component({
  selector: 'app-basecategoryprice',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, LoadingComponent],
  templateUrl: './basecategoryprice.component.html',
  styleUrl: './basecategoryprice.component.css',
})
export class BasecategorypriceComponent implements OnInit {
  baseCategory!: returnedCategory;
  baseCategoryPrice!: number;
  isLoading = true;

  newBaseCategoryForm!: FormGroup;
  newBaseCategoryFormControlNames!: FormKeys[];

  constructor(
    private categoryService: CategoryService,
    private efb: EnhancedFormBuilderService
  ) {}

  async ngOnInit(): Promise<void> {
    this.efb.createForm({
      Name: {
        type: 'text',
        displayName: 'اسم العيار',
        controls: ['', Validators.required],
      },
      Standard: {
        type: 'number',
        displayName: 'العيار',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
      Price: {
        type: 'number',
        displayName: 'السعر',
        controls: [0, [Validators.required, Validators.min(0)]],
      },
    });
    this.newBaseCategoryForm = this.efb.resultForm;

    this.newBaseCategoryFormControlNames = this.efb.getFormControlNames();

    const response = await this.categoryService.getBaseCategory();
    if (response.ok) {
      this.baseCategory = response.body!;
      this.baseCategoryPrice = this.baseCategory.price ?? 0;
      this.categoryService.changeCateogryPrices.emit();
    }
    this.isLoading = false;
  }

  async onSubmit(): Promise<void> {
    if (this.newBaseCategoryForm.valid) {
      const response = await this.categoryService.addCategory(
        this.newBaseCategoryForm.value
      );
      if (response.ok) {
        this.baseCategory = {
          ...response.body!,
        };
        this.baseCategoryPrice = this.baseCategory.price;
        this.categoryService.changeCateogryPrices.emit();
      }
    }
  }

  resetBaseCategoryPrice(): void {
    this.baseCategoryPrice = this.baseCategory?.price ?? 0;
  }

  async changeBaseCategoryPrice(): Promise<void> {
    const response = await this.categoryService.changeBaseCategoryPrice(
      this.baseCategoryPrice
    );
    if (response.ok) {
      this.baseCategory = {
        ...response.body!,
      };
      this.categoryService.changeCateogryPrices.emit();
    } else {
      console.error('Failed to change base category price');
    }
  }
}
