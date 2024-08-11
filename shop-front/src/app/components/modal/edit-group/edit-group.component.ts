import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Location } from '@angular/common';

import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { GroupService } from '../../../Services/group.service';
import { CategoryService } from '../../../Services/category.service';
import { LoadingComponent } from '../../loading/loading.component';
import { GroupInput, GroupOutput } from '../../../interfaces/group';

@Component({
  selector: 'app-edit-group',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent],
  templateUrl: './edit-group.component.html',
  styleUrl: './edit-group.component.css',
})
export class EditGroupComponent implements OnInit {
  static Path = 'edit-group';

  isLoading = true;

  editGroup!: GroupOutput;

  newGroupForm!: FormGroup;
  newGroupFormControlNames!: FormKeys[];

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private groupService: GroupService,
    private categoryService: CategoryService,
    private currentLocation: Location
  ) {}

  async ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const { data: group } = this.currentLocation.getState() as {
      data: GroupOutput;
    };

    this.editGroup = group;

    var categories = await this.categoryService.getAllCategories();
    if (!categories.ok) {
      console.log('error');
    }
    this.isLoading = false;
    var options = categories.body?.map((c) => ({
      label: c.name,
      value: c.id,
    }));
    this.efb.createForm({
      name: {
        type: 'text',
        displayName: 'أسم المجموعة',
        controls: [group.name, Validators.required],
      },
      categoryId: {
        type: 'select',
        displayName: 'العيار',
        options: options,
        controls: [group.category.id, [Validators.required, Validators.min(0)]],
      },
    });

    this.newGroupForm = this.efb.resultForm;
    this.newGroupFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    var res = await this.groupService.EditGroup(
      this.editGroup.id,
      this.newGroupForm.value
    );
    if (res.ok) {
      this.groupService.changeGroups.emit();
      this.modal.dismiss();
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
