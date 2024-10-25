using AutoMapper;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.ModelsOut;
using BenzForum.Models;

namespace BenzForum.Data.Factory
{
    public class ModelConvertor : IModelConvertor
    {
        private readonly IMapper _mapper;

        public ModelConvertor()
        {
            _mapper = InitializeMapper();
        }

        private IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBPost, PostResponse>();
                cfg.CreateMap<DBUser, UserResponse>();
                cfg.CreateMap<DBComment, CommentResponse>();
                cfg.CreateMap<PostRequest, DBPost>();
                cfg.CreateMap<CommentRequest, DBComment>();
            });

            return config.CreateMapper();
        }

        public TDestination Convert<TSource, TDestination>(TSource source) where TSource : class where TDestination : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source object cannot be null.");
            }

            return _mapper.Map<TDestination>(source);
        }
    }
}

