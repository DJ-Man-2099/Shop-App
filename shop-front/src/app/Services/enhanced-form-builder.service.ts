import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, ValidatorFn } from '@angular/forms';

type enhancedFormControlDef = {
  [name: string]: {
    type: string;
    displayName: string;
    options?: { value: any; label: string }[];
    controls: [any, ValidatorFn | ValidatorFn[] | null | undefined];
  };
};

type formControlDef = {
  [name: string]: [any, ValidatorFn | ValidatorFn[] | null | undefined];
};

export type FormKeys = {
  key: string;
  type: string;
  displayName: string;
  options?: { value: any; label: string }[];
};

@Injectable({
  providedIn: 'root',
})
export class EnhancedFormBuilderService {
  resultForm!: FormGroup;
  origControls!: enhancedFormControlDef;
  constructor(private fb: FormBuilder) {}

  createForm(formControls: enhancedFormControlDef) {
    this.origControls = formControls;
    const tempControls = Object.keys(formControls).reduce<formControlDef>(
      (acc, key) => {
        acc[key] = formControls[key].controls;

        return acc;
      },
      {}
    );
    this.resultForm = this.fb.group(tempControls);
  }

  getFormControlNames() {
    return Object.keys(this.origControls).reduce<FormKeys[]>((acc, key) => {
      const type = this.origControls[key].type;
      const displayName = this.origControls[key].displayName;
      const options = this.origControls[key].options;
      acc.push({ key, type, displayName, options });
      return acc;
    }, []);
  }
}
