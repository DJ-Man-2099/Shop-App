import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { CategoryService } from '../../Services/category.service';
import { BaseCategoryInfo, FormKeys } from '../../interfaces/category';

@Component({
  selector: 'app-basecategoryprice',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './basecategoryprice.component.html',
  styleUrl: './basecategoryprice.component.css',
})
export class BasecategorypriceComponent implements OnInit {
  baseCategory?: BaseCategoryInfo;
  baseCategoryPrice!: number;
  isLoading = true;

  newBaseCategoryForm!: FormGroup;
  newBaseCategoryFormControlNames!: FormKeys[];

  constructor(
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.newBaseCategoryForm = this.fb.group({
      Name: ['', Validators.required],
      Standard: [0, [Validators.required, Validators.min(0)]],
      Price: [0, [Validators.required, Validators.min(0)]],
    });
    this.newBaseCategoryFormControlNames = Object.keys(
      this.newBaseCategoryForm.controls
    ).reduce<FormKeys[]>((acc, key) => {
      const type =
        typeof this.newBaseCategoryForm.controls[key].value === 'string'
          ? 'text'
          : 'number';
      const name =
        key === 'Name' ? 'اسم العيار' : key === 'Standard' ? 'العيار' : 'السعر';
      acc.push({ key, type, name });
      return acc;
    }, []);
    this.categoryService.getBaseCategory().subscribe({
      next: (response) => {
        if (response.body) {
          this.baseCategory = response.body as {
            standard: number;
            price: number;
          };
          this.baseCategoryPrice = this.baseCategory.price ?? 0;
          this.cdr.detectChanges();
        } else {
          console.error('Response body is null');
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error(error);
        this.isLoading = false;
      },
    });
  }

  async onSubmit(): Promise<void> {
    if (this.newBaseCategoryForm.valid) {
      const response = await this.categoryService.addBaseCategory(
        this.newBaseCategoryForm.value
      );
      if (response.ok) {
        console.log(response.body);
        this.baseCategory = {
          standard: response.body!.standard,
          price: response.body!.price,
        };
        this.baseCategoryPrice = this.baseCategory.price;
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
    console.log(response.body);
    if (response.ok) {
      this.baseCategory = {
        standard: response.body!.standard,
        price: response.body!.price,
      };
    } else {
      console.error('Failed to change base category price');
    }
  }
}
