import { Component, OnDestroy } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent implements OnDestroy{
  blogModel: AddBlogPost;
  blogPostSubscription?: Subscription;

  constructor(private blogPostService: BlogPostService,
    private router: Router) {
    this.blogModel = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date()
    }
  }
  
  onFormSubmit(): void {
    this.blogPostSubscription = this.blogPostService.createBlogPost(this.blogModel)
      .subscribe({
        next: (response)=>{
          this.router.navigateByUrl('/admin/blogposts');
        }
      });
  }

  ngOnDestroy(): void {
    this.blogPostSubscription?.unsubscribe();
  }
}
