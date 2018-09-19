using System;
using System.Collections;
using System.Collections.Generic;
using PAT.Common.Classes.Expressions.ExpressionClass;

//the namespace must be PAT.Lib, the class and method names can be arbitrary
namespace PAT.Lib {

    public class Signature: ExpressionValue {
        public int signature;
        /// Default constructor without any parameter must be implemented
        public Signature() {
            signature = -1;
        }

        public Signature(int sig) {
            signature = sig;
        }
        
        public Signature(Signature sig) {
            signature = sig.GetValue();
        }

        public void SetValue(int sig) {
            signature = sig;
        }

        public int GetValue() {
            return signature;
        }

        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return signature.ToString();
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new Signature(signature);
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
           get {
               return signature.ToString();
           }
        }
    }

    public class Block : ExpressionValue {
        public int hash;
        public SignatureList signatures;
        /// Default constructor without any parameter must be implemented
        public Block() {
            hash = -1;
            signatures = new SignatureList();
        }

        public Block(int blockHash) {
            hash = blockHash;
            signatures = new SignatureList();
        }

        public Block(int blockHash, SignatureList sigs) {
            hash = blockHash;
            signatures = new SignatureList(new List<Signature>(sigs.GetList()));
        }

        public void SetSignatureList(SignatureList sigs) {
            signatures = new SignatureList(new List<Signature>(sigs.GetList()));
        }

        public SignatureList GetSignatureList() {
            return signatures;
        }
        
        public void SetHash(int blockHash) {
            hash = blockHash;
        }

        public int GetHash() {
            return hash;
        }

        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new Block(hash, new SignatureList(new List<Signature>(signatures.GetList())));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
           get {
               return "B" + hash;
           }
        }
    }

    public class Proposal : ExpressionValue {
        public Block block;
        public Signature signature;

        /// Default constructor without any parameter must be implemented
        public Proposal() {
            block = new Block();
            signature = new Signature();
        }

        public Proposal(Block b, Signature sig) {
            block = new Block(b.GetHash());
            signature = new Signature(sig);
        }

        public Block GetBlock() {
            return block;
        }
        
        public int GetSignature() {
            return signature.GetValue();
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new Proposal(block, signature);
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
           get {
               return "Proposal [B" + block.GetHash() + " by P" + signature.ToString() + "]";
           }
        }

        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return ExpressionID;
        }
    }

    public class Vote : ExpressionValue, IComparable {
        public int blockHash;
        public Signature signature;

        /// Default constructor without any parameter must be implemented
        public Vote() {
            blockHash = -1;
            signature = new Signature();
        }

        public Vote(int bh, Signature sig) {
            blockHash = bh;
            signature = new Signature(sig);
        }

        public int GetBlockHash() {
            return blockHash;
        }
        
        public int GetSignature() {
            return signature.GetValue();
        }

        int IComparable.CompareTo(object obj) {
            Vote v = (Vote) obj;
            return String.Compare(this.ToString(), v.ToString());
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new Vote(blockHash, signature);
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification
        public override string ExpressionID {
           get {
               return "Vote [B" + blockHash + " by V" + signature.ToString() + "]";
           }
        }

        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return ExpressionID;
        }
    }
    
    public class BlockChain : ExpressionValue {
        public List<Block> chain;
        
        public BlockChain() {
            this.chain = new List<Block>();
        }

        public BlockChain(List<Block> chain) {
            this.chain = chain;            
        }
        
        public void Add(Block block) {
            this.chain.Add(block);
        }

        public bool Contains(Block block) {
            foreach (Block b in chain) {
                if(b.GetHash() == block.GetHash()) {
                    return true;
                }
            }
            return false;
        }

        public bool IsEmpty() {
            return this.chain.Count == 0;
        }

        public bool ContainsDuplicates() {
            List<Block> tmpList = new List<Block>();
            foreach (Block b in chain) {
                if (!tmpList.Contains(b)) {
                    tmpList.Add(b);
                } else {
                    return true;
                }
            }
            return false;
        }

        public Block Get(int index) {
            return this.chain[index];
        }

        public Block Peek() {
            if(this.chain.Count == 0) {
                return new Block(-1);
            }
            return this.chain[this.chain.Count-1];
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new BlockChain(new System.Collections.Generic.List<Block>(this.chain));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
            get {
                string returnString = "";
                foreach (Block b in chain) {
                    returnString += b.GetHash() + ",";
                }
                return returnString;
            }
        }
        
        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }
    }

    public class VoteSet : ExpressionValue {
        public SortedList<Vote> set;
        public Dictionary<int, int> counter;

        public VoteSet() {
            this.set = new SortedList<Vote>();
            this.counter = new Dictionary<int, int>();
        }

        public VoteSet(SortedList<Vote> set, Dictionary<int, int> counter) {
            this.set = set;
            this.counter = counter;
        }
        
        public int Size() {
            return this.set.Count;
        }

        public void Clear() {
            set.Clear();
            counter.Clear();
        }

        public void Add(Vote vote) {
            bool contains = false;
            foreach (Vote v in set) {
                if(vote.GetBlockHash() == v.GetBlockHash() && vote.GetSignature() == v.GetSignature()) {
                    contains = true;
                    break;
                }
            }
            if(!contains) {
                set.Add(vote);
                if(counter.ContainsKey(vote.GetBlockHash())) {
                    counter[vote.GetBlockHash()]++;
                } else {
                    counter.Add(vote.GetBlockHash(), 1);
                }
            }
        }

        public SignatureList getSignaturesForBlock(int blockHash) {
            List<Signature> signatures = new List<Signature>();
            foreach (Vote v in set) {
                if(v.GetBlockHash() == blockHash) {
                    signatures.Add(new Signature(v.GetSignature()));
                }
            }
            return new SignatureList(new List<Signature>(signatures));
        }

        public Block GetFirstBlockWithMinSupport(int min) {
            foreach(KeyValuePair<int, int> entry in counter) {
                if(entry.Value >= min) {
                    return new Block(entry.Key);
                }
            }
            return new Block();
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new VoteSet(new SortedList<Vote>(new List<Vote>(this.set.GetInternalList())), new Dictionary<int, int>(this.counter));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
            get {
                string returnString = "";
                foreach (KeyValuePair<int, int> kvp in counter) {
                    returnString += string.Format("{0}:{1};", kvp.Key, kvp.Value);
                }
                foreach (Vote v in set) {
                    returnString += v.ToString() + ",";
                }
                return returnString;
            }
        }
        
        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }
    }

    public class ProposalList : ExpressionValue {
        public List<Proposal> list;
        
        public ProposalList() {
            this.list = new List<Proposal>();
        }

        public ProposalList(List<Proposal> list) {
            this.list = list;            
        }
        
        public void Set(int index, Proposal proposal) {
            while(index >= list.Count) {
               this.list.Add(new Proposal()); 
            }
            this.list[index] = proposal;
        }

        public Proposal Get(int index) {
            return this.list[index];
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new ProposalList(new List<Proposal>(this.list));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
            get {
                string returnString = "";
                foreach (Proposal p in list) {
                    returnString += p.ToString() + ",";
                }
                return returnString;
               }
        }
        
        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }
    }

    public class BlockList : ExpressionValue {
        public List<Block> list;
        
        public BlockList() {
            this.list = new List<Block>();
        }

        public BlockList(List<Block> list) {
            this.list = list;            
        }
        
        public void Set(int index, Block block) {
            while(index >= list.Count) {
               this.list.Add(new Block()); 
            }
            this.list[index] = block;
        }

        public Block Get(int index) {
            if(index >= list.Count) {
                return new Block();
            }
            return this.list[index];
        }

        public void Clear() {
            this.list.Clear();
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new BlockList(new List<Block>(this.list));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
            get {
                string returnString = "";
                foreach (Block b in list) {
                    returnString += b.ToString() + ",";
                }
                return returnString;
               }
        }
        
        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }
    }

    public class SignatureList : ExpressionValue {
        public List<Signature> list;
        
        public SignatureList() {
            this.list = new List<Signature>();
        }

        public SignatureList(List<Signature> list) {
            this.list = list;            
        }

        public List<Signature> GetList() {
            return this.list;
        }

        /// Return a deep clone of the hash table
        /// NOTE: this must be a deep clone, shallow clone may lead to strange behaviors.
        /// This method must be overriden
        public override ExpressionValue GetClone() {
            return new SignatureList(new List<Signature>(this.list));
        }

        /// Return the compact string representation of the hash table.
        /// This method must be overriden
        /// Smart implementation of this method can reduce the state space and speedup verification 
        public override string ExpressionID {
            get {
                string returnString = "";
                foreach (Signature s in list) {
                    returnString += s.ToString() + ",";
                }
                return returnString;
               }
        }
        
        /// Return the string representation of the hash table.
        /// This method must be overriden
        public override string ToString() {
            return "[" + ExpressionID + "]";
        }
    }

    public class SortedList<T> : ICollection<T> {
        private List<T> m_innerList;
        private Comparer<T> m_comparer;
    
        public SortedList() : this(new List<T>()) {}
        
        public SortedList(List<T> m_innerList) {
            this.m_innerList = m_innerList;
            this.m_comparer = Comparer<T>.Default;
        }
        
        public List<T> GetInternalList() {
            return  m_innerList;
        }
    
        public void Add(T item) {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            m_innerList.Insert(insertIndex, item);
        }
    
        public bool Contains(T item) {
            return IndexOf(item) != -1;
        }
    
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire SortedList<T>
        /// </summary>
        public int IndexOf(T item) {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            if (insertIndex == m_innerList.Count) {
                return -1;
            }
            if (m_comparer.Compare(item, m_innerList[insertIndex]) == 0) {
                int index = insertIndex;
                while (index > 0 && m_comparer.Compare(item, m_innerList[index - 1]) == 0) {
                    index--;
                }
                return index;
            }
            return -1;
        }
    
        public bool Remove(T item) {
            int index = IndexOf(item);
            if (index >= 0) {
                m_innerList.RemoveAt(index);
                return true;
            }
            return false;
        }
    
        public void RemoveAt(int index) {
            m_innerList.RemoveAt(index);
        }
    
        public void CopyTo(T[] array) {
            m_innerList.CopyTo(array);
        }
    
        public void CopyTo(T[] array, int arrayIndex) {
            m_innerList.CopyTo(array, arrayIndex);
        }
    
        public void Clear() {
            m_innerList.Clear();
        }
    
        public T this[int index] {
            get {
                return m_innerList[index];
            }
        }
    
        public IEnumerator<T> GetEnumerator() {
            return m_innerList.GetEnumerator();
        }
    
        IEnumerator IEnumerable.GetEnumerator() {
            return m_innerList.GetEnumerator();
        }
    
        public int Count {
            get {
                return m_innerList.Count;
            }
        }
    
        public bool IsReadOnly {
            get {
                return false;
            }
        }
    
        public static int FindIndexForSortedInsert(List<T> list, Comparer<T> comparer, T item) {
            if (list.Count == 0) {
                return 0;
            }
    
            int lowerIndex = 0;
            int upperIndex = list.Count - 1;
            int comparisonResult;
            while (lowerIndex < upperIndex) {
                int middleIndex = (lowerIndex + upperIndex) / 2;
                T middle = list[middleIndex];
                comparisonResult = comparer.Compare(middle, item);
                if (comparisonResult == 0) {
                    return middleIndex;
                } else if (comparisonResult > 0) {  // middle > item
                    upperIndex = middleIndex - 1;
                } else {    // middle < item
                    lowerIndex = middleIndex + 1;
                }
            }
    
            // At this point any entry following 'middle' is greater than 'item',
            // and any entry preceding 'middle' is lesser than 'item'.
            // So we either put 'item' before or after 'middle'.
            comparisonResult = comparer.Compare(list[lowerIndex], item);
            if (comparisonResult < 0) { // middle < item 
                return lowerIndex + 1;
            } else {
                return lowerIndex;
            }
        }
    }
}