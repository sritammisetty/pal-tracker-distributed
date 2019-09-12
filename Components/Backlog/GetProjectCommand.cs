using System;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Backlog
{
    public class GetProjectCommand : HystrixCommand<ProjectInfo>
    {
        private readonly Func<long, Task<ProjectInfo>> _getProjectFn;
        private readonly Func<long, Task<ProjectInfo>> _getProjectFallbackFn;
        private readonly long _projectId;
        
        public GetProjectCommand(
            Func<long, Task<ProjectInfo>> getProjectFn,
            Func<long, Task<ProjectInfo>> getProjectFallbackFn,
            long projectId
        ) : base(HystrixCommandGroupKeyDefault.AsKey("ProjectClientGroup"))
        {
            _getProjectFn = getProjectFn;
            _getProjectFallbackFn = getProjectFallbackFn;
            _projectId = projectId;
        }

        protected override async Task<ProjectInfo> RunAsync() => await _getProjectFn(_projectId);
        protected override async Task<ProjectInfo> RunFallbackAsync() => await _getProjectFallbackFn(_projectId);
    }
}