import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit, OnDestroy {

  id: string | null = null;
  routeSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  blogPostModel?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];

  constructor(private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router) { }


  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        //Get blogpost from api
        if (this.id) {
          this.blogPostService.getBlogPostById(this.id).subscribe({
            next: (response) => {
              this.blogPostModel = response;
              this.selectedCategories = response.categories.map(x => x.id);
            }
          })
        }
      }
    })
  }

  onFormSubmit(): void {
    //convert currentblogPostModel to Request Object
    if (this.blogPostModel && this.id) {
      let updateBlogPost: UpdateBlogPost = {
        author: this.blogPostModel.author,
        content: this.blogPostModel.content,
        shortDescription: this.blogPostModel.shortDescription,
        featuredImageUrl: this.blogPostModel.featuredImageUrl,
        isVisible: this.blogPostModel.isVisible,
        publishedDate: this.blogPostModel.publishedDate,
        title: this.blogPostModel.title,
        urlHandle: this.blogPostModel.urlHandle,
        categories: this.selectedCategories ?? []
      };

      this.updateBlogPostSubscription = this.blogPostService.UpdateBlogPost(this.id, updateBlogPost).subscribe(
        {
          next: (response)=>{
            this.router.navigateByUrl('/admin/blogposts');
          }
        }
      )
    }
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
  }

}
