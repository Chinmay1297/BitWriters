import { Component } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent {

  categoryRequestModel: AddCategoryRequest;

  constructor(private categoryService: CategoryService) {
    this.categoryRequestModel = {
      name: '',
      urlHandle: ''
    };
  }

  onFormSubmit() {
    this.categoryService.addCategory(this.categoryRequestModel)
      .subscribe({
        next: (response) => {
          console.log('This was successful');
        },
        error: (error) => {
          console.log("Error occured: ", error);
        },
        complete: () => {

        }
      });
  }
}
