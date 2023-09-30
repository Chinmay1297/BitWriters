import { Component } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent {

  categoryRequestModel: AddCategoryRequest;

  constructor() {
    this.categoryRequestModel = {
      name: '',
      urlHandle: ''
    };
  }

  onFormSubmit() {
    console.log(this.categoryRequestModel);
  }
}
