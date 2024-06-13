using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using Services;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostServices postServices;

        public PostsController()
        {
            postServices = new PostServices();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPost(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = postServices.GetPost(pageNumber, pageSize);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("Search-Post")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> SearchPost(
            [FromQuery] string searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = postServices.PostPagedResult(searchTerm, pageNumber, pageSize);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
            var post = postServices.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO post)
        {
            postServices.UpdatePost(id, post);
            return Ok();
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO post)
        {
            postServices.CreatePost(post);
            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = postServices.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            postServices.DeletePost(id);
            return NoContent();
        }

        [HttpPost("UploadPostImage/{postId}")]
        public async Task<IActionResult> UploadCourtImage(int postId, [FromBody] Base64ImageModel model)
        {
            if (string.IsNullOrEmpty(model.Base64Image))
                return BadRequest("No image uploaded.");

            await postServices.UploadPostImage(postId, model.Base64Image);

            return Ok("Image uploaded successfully.");
        }
    }
}
