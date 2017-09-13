using System;
using System.Linq;
using System.Linq.Expressions;

namespace Shoppi.Data.Specifications
{
    public class Specification<T>
    {
        private Expression<Func<T, bool>> _predicate;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _sort;
        private Func<IQueryable<T>, IQueryable<T>> _postProcess;

        public Specification(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }

        public Specification<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var newSpecification = new Specification<T>(_predicate) { _postProcess = _postProcess };

            if (_sort != null)
            {
                newSpecification._sort = x => _sort(x).ThenBy(property);
            }
            else
            {
                newSpecification._sort = x => x.OrderBy(property);
            }

            return newSpecification;
        }

        public Specification<T> OrderByDescending<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var newSpecification = new Specification<T>(_predicate) { _postProcess = _postProcess };

            if (_sort != null)
            {
                newSpecification._sort = x => _sort(x).ThenByDescending(property);
            }
            else
            {
                newSpecification._sort = x => x.OrderByDescending(property);
            }

            return newSpecification;
        }

        public Specification<T> Skip(int amount)
        {
            var newSpecification = new Specification<T>(_predicate) { _sort = _sort };

            if (_postProcess != null)
            {
                newSpecification._postProcess = x => _postProcess(x).Skip(amount);
            }
            else
            {
                newSpecification._postProcess = x => x.Skip(amount);
            }

            return newSpecification;
        }

        public Specification<T> Take(int amount)
        {
            var newSpecification = new Specification<T>(_predicate) { _sort = _sort };

            if (_postProcess != null)
            {
                newSpecification._postProcess = x => _postProcess(x).Take(amount);
            }
            else
            {
                newSpecification._postProcess = x => x.Take(amount);
            }

            return newSpecification;
        }

        public IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query)
        {
            return Prepare(query);
        }

        private IQueryable<T> Prepare(IQueryable<T> query)
        {
            var result = query.Where(_predicate);

            if (_sort != null)
            {
                result = _sort(result);
            }
            if (_postProcess != null)
            {
                result = _postProcess(result);
            }

            return result;
        }
    }
}